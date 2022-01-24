using Amazon.CDK;
using Amazon.CDK.Pipelines;
using Amazon.CDK.AWS.CodeCommit;
using Constructs;
using Amazon.CDK.AWS.CodePipeline.Actions;
using System.Collections.Generic;
using Amazon.CDK.AWS.CodeBuild;
using Amazon.CDK.AWS.CodePipeline;

namespace DioCdkBuild
{
    public class DioCdkBuildStack : Stack
    {
        internal DioCdkBuildStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            /*
            Pipeline pipeline = new Pipeline(this, "MyPipeline");
            Artifact sourceOutput = new Artifact();
            GitHubSourceAction sourceAction = new GitHubSourceAction(new GitHubSourceActionProps {
                ActionName = "GitHub_Source",
                Owner = "awslabs",
                Repo = "aws-cdk",
                OauthToken = SecretValue.SecretsManager("my-github-token"),
                Output = sourceOutput,
                Branch = "develop"
            });
            pipeline.AddStage(new StageOptions {
                StageName = "Source",
                Actions = new [] { sourceAction }
            });    
            */
     

            /* BGF: Python example. 
             project = codebuild.Project(
            self,
            "MyProject",
            # You'll need to configure this first. See:
            # https://docs.aws.amazon.com/cdk/api/latest/docs/aws-codebuild-readme.html#githubsource-and-githubenterprisesource
                source=codebuild.Source.git_hub(
                owner="blimmer", repo="my_repo", webhook=True
            ),
            # Configure your project here
            )
            */
  
            var repo = new Repository(this, "DioRepository", new RepositoryProps
            {
                RepositoryName = "DioRepository"
            });

            var pipeline = new CodePipeline(this, "Pipeline", new CodePipelineProps
            {
                PipelineName = "DioPipeline",

                // Builds our source code outlined above into a could assembly artifact
                Synth = new ShellStep("Synth", new ShellStepProps{
                    Input = CodePipelineSource.CodeCommit(repo, "master"),  // Where to get source code to build
                    Commands = new string[] {
                        "npm install -g aws-cdk",
                        "sudo apt-get install -y dotnet-sdk-3.1", // Language-specific install cmd
                        "dotnet build"  // Language-specific build cmd
                    }
                }),

            });

            
            var deploy = new WorkshopPipelineStage(this, "Deploy");
            var deployStage = pipeline.AddStage(deploy);
            
        }
    }
}
