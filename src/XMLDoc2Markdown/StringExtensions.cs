using System;
using System.Runtime.InteropServices;

namespace XMLDoc2Markdown
{
    internal static class StringExtensions
    {
        internal static string FormatChevrons(this string value)
        {
            return value.Replace("<", "&lt;").Replace(">", "&gt;");
        }

        private static readonly string[] typeNameToFileName_oldValues = {"<", ">", ",", " ", "`"};
        private static readonly string[] typeNameToFileName_newValues = {"{", "}", String.Empty, "-", "-"};
        internal static string MakeTypeNameFileNameSafe(string typeName)
        {
            return typeName.ReplaceMany(typeNameToFileName_oldValues, typeNameToFileName_newValues, typeName.Length);
        }

        // Source: https://www.codeproject.com/Articles/310525/The-ReplaceMany-Method
        internal static unsafe string ReplaceMany(this string str, string[] oldValues, string[] newValues, int resultStringLength)
        {
            int oldCount = oldValues.Length;
            int newCount = newValues.Length;
            if (oldCount != newCount)
                throw new ArgumentException("Each old value must match exactly one new value");
            for (int i = 0; i < oldCount; i++)
            {
                if (string.IsNullOrEmpty(oldValues[i]))
                    throw new ArgumentException("Old value may not be null or empty.", "oldValues");
                if (newValues[i] == null)
                    throw new ArgumentException("New value may be null.", "newValues");
            }
            int strLen = str.Length;
            int buildSpace = resultStringLength == 0 ? strLen << 1 : resultStringLength;
            // A hand-made StringBuilder is here
            char[] buildArr = new char[buildSpace];        
            // cached pinned pointers
            GCHandle buildHandle = GCHandle.Alloc(buildArr, GCHandleType.Pinned);
            GCHandle[] oldHandles = new GCHandle[oldCount];
            GCHandle[] newHandles = new GCHandle[newCount];
            int* newLens = stackalloc int[newCount];
            int* oldLens = stackalloc int[newCount];
            char** oldPtrs = stackalloc char*[newCount];
            char** newPtrs = stackalloc char*[newCount];
            // other caches
            for (int i = 0; i < oldCount; i++)
            {
                oldHandles[i] = GCHandle.Alloc(oldValues[i], GCHandleType.Pinned);
                newHandles[i] = GCHandle.Alloc(newValues[i], GCHandleType.Pinned);
                oldPtrs[i] = (char*)oldHandles[i].AddrOfPinnedObject();
                newPtrs[i] = (char*)newHandles[i].AddrOfPinnedObject();
                newLens[i] = newValues[i].Length;
                oldLens[i] = oldValues[i].Length;
            }
            int buildIndex = 0;
            fixed (char* _strFix = str)
            {
                char* build = (char*)buildHandle.AddrOfPinnedObject();
                char* pBuild = build;
                char* pStr = _strFix;
                char* endStr = pStr + strLen;
                char* copyStartPos = pStr;
                while (pStr != endStr)
                {
                    bool find = false;
                    for (int i = 0; i < oldCount; ++i)
                    {
                        int oldValLen = *(oldLens+i);
                        // if the string to find does not exceed the original string
                        if (oldValLen > 0 && pStr + oldValLen <= endStr)
                        {
                            char* _oldFix = *(oldPtrs + i);
                            if (*pStr == *_oldFix) // check the first char
                            {
                                // compare the rest. First, compare the second character
                                find = oldValLen == 1;
                                if (!find)
                                {
                                    if (*(pStr + 1) == *(_oldFix + 1))
                                        find = oldValLen == 2
                                        // use native memcmp function.
                                        || 0 == memcmp((byte*)(pStr + 2), (byte*)(_oldFix + 2), (oldValLen - 2) << 1);
                                }
                                        
                                if (find)
                                {
                                    int newValLen = newLens[i];
                                    char* newFix = newPtrs[i];
                                    int copyLen = (int)(pStr - copyStartPos);
                                    // allocate new space if needed.
                                    if (buildIndex + newValLen + copyLen > buildSpace)
                                    {
                                        buildHandle.Free();
                                        int oldSpace = buildSpace;
                                        buildSpace = Math.Max((int)(buildIndex + newValLen + copyLen), buildSpace << 1);
                                        buildArr = ExpandArray(buildArr, oldSpace, buildSpace);
                                        buildHandle = GCHandle.Alloc(buildArr, GCHandleType.Pinned);
                                        build = (char*)buildHandle.AddrOfPinnedObject();
                                        pBuild = build + buildIndex;
                                    }
                                    // if there is a part from the original string to copy, then do it.
                                    if (copyLen > 0)
                                    {
                                        memcpy((byte*)(pBuild), (byte*)copyStartPos, copyLen << 1);
                                        buildIndex += copyLen;
                                        pBuild = build + buildIndex;
                                    }
                                    // append the replacement to the builder
                                    memcpy((byte*)(pBuild), (byte*)newFix, newValLen << 1);
                                    pBuild += newValLen;
                                    buildIndex += newValLen;
                                    pStr += oldValLen;
                                    copyStartPos = pStr;
                                    // this is redutant, but brings more determinism to a method's behaviour.
                                    break;
                                }
                            }
                        }
                    }
                    // if not found, just increment the pointer within the main string
                    if (!find)
                        pStr++;
                }
                // if there is a part from the original string to copy, then do it.
                if (copyStartPos != pStr)
                {
                    int copyLen = (int)(pStr - copyStartPos);
                    // again, allocate new space if needed
                    if (buildIndex + copyLen > buildSpace)
                    {
                        buildHandle.Free();
                        int oldSpace = buildSpace;
                        buildSpace = Math.Max((int)(buildIndex + copyLen), buildSpace << 1);
                        buildArr = ExpandArray(buildArr, oldSpace, buildSpace);
                        buildHandle = GCHandle.Alloc(buildArr, GCHandleType.Pinned);
                        build = (char*)buildHandle.AddrOfPinnedObject();
                        pBuild = build + buildIndex;
                    }
                    // append the ending
                    memcpy((byte*)(pBuild), (byte*)copyStartPos, copyLen << 1);
                    buildIndex += copyLen;
                }
            }
            // unpin string handles
            for (int i = 0; i < newCount; i++)
            {
                oldHandles[i].Free();
                newHandles[i].Free();
            }
            buildHandle.Free();
            return new string(buildArr, 0, buildIndex);
        }

        internal static unsafe char[] ExpandArray(char[] array, int oldSize, int newSize)
        {
            if (oldSize > newSize || oldSize > array.Length)
                throw new ArgumentOutOfRangeException();
            char[] bigger;
            bigger = new char[newSize];
            fixed (char* bpt = bigger, apt = array)
                memcpy((byte*)bpt, (byte*)apt, oldSize<<1);
            return bigger;
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int memcmp(byte* b1, byte* b2, int count);
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int memcpy(byte* dest, byte* src, int count);
    }
}
