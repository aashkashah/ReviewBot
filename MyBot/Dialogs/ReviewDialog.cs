using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace MyBot.Dialogs
{
    public class ReviewDialog : ComponentDialog
    {   
        private const string Done = "done";
        private const string MovieSelected = "movieSelected";
        private const string Review = "review";
        private readonly string[] _movieOptions = new string[]
        {
            "Harry Potter", "Star Wars", "Avengers", Done,
        };

        public ReviewDialog(): base(nameof(ReviewDialog))
        {
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {
                    SelectionStepAsync,
                    WriteReviewAsync,
                    LoopStepAsync,
                }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> SelectionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {   
            //var list = new List<string>();
            //stepContext.Values[MovieSelected] = list;

            string message = $"Please choose a movie to review, or `{Done}` to finish.";
            var options =_movieOptions.ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(message),
                RetryPrompt = MessageFactory.Text("Please choose a movie from the list."),
                Choices = ChoiceFactory.ToChoices(options),
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private static async Task<DialogTurnResult> WriteReviewAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = stepContext.Context.Activity.Text.ToLowerInvariant();

            if (choice == Done)
            {   
                return await stepContext.EndDialogAsync(choice, cancellationToken);
            }

            var movieChoice = (FoundChoice)stepContext.Result;
            stepContext.Values[MovieSelected] = movieChoice.Value;
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your review.") };

            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> LoopStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var review = stepContext.Result.ToString();
            stepContext.Values[Review] = review;

            return await stepContext.EndDialogAsync(review, cancellationToken);
        }
    }
}
