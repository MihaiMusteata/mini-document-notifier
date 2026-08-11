namespace MiniDocumentNotifier.Infrastructure.Concurrency
{
    public static class Constants
    {
        public const string WinFormsMutexName = @"Global\MiniDocumentNotifier.WinForms";
        public const string BackgroundAppMutexName = @"Global\MiniDocumentNotifier.BackgroundApp";
        public const string BackgroundAppSemaphoreName = @"Global\MiniDocumentNotifier.BackgroundApp.Signal";
    }
}