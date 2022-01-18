using Amazon.CDK;

namespace DioCdkBuild
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new DioCdkBuildStack(app, "DioCdkBuildStack");

            app.Synth();
        }
    }
}
