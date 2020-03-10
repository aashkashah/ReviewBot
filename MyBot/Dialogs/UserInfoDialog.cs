
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace MyBot.Dialogs
{
    public class UserInfoDialog : ComponentDialog
    {
        private const string UserInfo = "userInfo";

        public UserInfoDialog()
            : base(nameof(UserInfoDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>)));

            AddDialog(new ReviewDialog());

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                NameStepAsync,
                AgeStepAsync,
                ReviewStepAsync,
                AcknowledgementStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private static async Task<DialogTurnResult> NameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {  
            stepContext.Values[UserInfo] = new UserProfile();

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your name.") };
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> AgeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {   
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.Name = (string)stepContext.Result;
            stepContext.Values[UserInfo] = userProfile;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your age.") };
            return await stepContext.PromptAsync(nameof(NumberPrompt<int>), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ReviewStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {   
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.Age = (int)stepContext.Result;
            stepContext.Values[UserInfo] = userProfile;

            return await stepContext.BeginDialogAsync(nameof(ReviewDialog), null, cancellationToken);
        }

        private async Task<DialogTurnResult> AcknowledgementStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {   
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.MovieReview = stepContext.Result as string;

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Thanks for reviewing, {((UserProfile)stepContext.Values[UserInfo]).Name}."),
                cancellationToken);

            return await stepContext.EndDialogAsync(stepContext.Values[UserInfo], cancellationToken);
        }
    }
}
