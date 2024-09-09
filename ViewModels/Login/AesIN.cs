namespace BPMPlus.ViewModels.Login
{
    public class AesIN
    {
        public string AESKey { get; set; }
        public string AesIVKey { get; set; }
        public string PlainText { get; set; }
        public string CiperText { get; set; }
        public string CipherToPlainText { get; set; }
    }
}
