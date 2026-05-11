using MyClassLib;
using XMLDoc2Markdown.Signatures;
using XMLDoc2Markdown.Signatures.Modifiers;

namespace XMLDoc2Markdown.Tests.Signatures.Modifiers;

[Collection(nameof(SampleAssemblyCollection))]
public class RecordDetectionTests
{
    [Fact]
    public void IsRecordClass_true_for_record_class()
    {
        Assert.True(RecordDetection.IsRecordClass(typeof(Person)));
    }

    [Fact]
    public void IsRecordClass_false_for_plain_class()
    {
        Assert.False(RecordDetection.IsRecordClass(typeof(MyClass)));
    }

    [Fact]
    public void IsRecordClass_false_for_record_struct()
    {
        Assert.False(RecordDetection.IsRecordClass(typeof(Point)));
    }

    [Fact]
    public void IsRecordStruct_true_for_readonly_record_struct()
    {
        Assert.True(RecordDetection.IsRecordStruct(typeof(Point)));
    }

    [Fact]
    public void IsRecordStruct_false_for_plain_struct()
    {
        Assert.False(RecordDetection.IsRecordStruct(typeof(MyReadonlyStruct)));
    }

    [Fact]
    public void IsRecord_true_for_class_and_struct_records()
    {
        Assert.True(RecordDetection.IsRecord(typeof(Person)));
        Assert.True(RecordDetection.IsRecord(typeof(Point)));
    }

    [Fact]
    public void GetSignature_record_class_emits_record_class_keyword()
    {
        string sig = typeof(Person).GetSignature(full: true);
        Assert.Contains("record class", sig);
        Assert.Contains("Person", sig);
    }

    [Fact]
    public void GetSignature_record_struct_emits_readonly_record_struct_keyword()
    {
        string sig = typeof(Point).GetSignature(full: true);
        Assert.Contains("readonly", sig);
        Assert.Contains("record struct", sig);
    }
}
