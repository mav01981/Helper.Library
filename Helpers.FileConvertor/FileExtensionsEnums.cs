using System.ComponentModel;

namespace Helper.FileConvertor
{
    public enum FileExtensionsEnums
    {
        [Description(".doc")]
        MicrosoftWordDocument,
        [Description(".xlsx")]
        MicrosoftExcelWorkSheet,
        [Description(".pdf")]
        PDF,
        [Description(".txt")]
        TextFile
    }
}
