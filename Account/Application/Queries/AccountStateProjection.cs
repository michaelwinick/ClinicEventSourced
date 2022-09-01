using Account.Domain;
using Eventuous.Projections.MongoDB;
using MongoDB.Driver;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Account.Application.Queries;

public class AccountStateProjection : MongoProjection<AccountDocument>
{
    public AccountStateProjection(IMongoDatabase database) : base(database)
    {
        On<AccountEvents.V1.PersonalAccountCreationStarted>(stream => stream.GetId(), 
            (ctx, update) =>
            {
                var evt = ctx.Message;

                return update.SetOnInsert(x => x.Id, ctx.Stream.GetId())
                    .Set(x => x.AccountId, evt.AccountId)
                    .Set(x => x.AccountType, evt.AccountType)
                    .Set(x => x.State, evt.State);
            });

        On<AccountEvents.V1.PersonalAccountInformationAdded>(
            account => account
                .UpdateOne
                .IdFromStream(stream => stream.GetId())
                .Update((evt, update) => 
                    update
                        .Set(x => x.FirstName, evt.FirstName)
                        .Set(x => x.LastName, evt.LastName)
                        .Set(x => x.Dob, evt.Dob)
                        .Set(x => x.State, evt.State)));

        On<AccountEvents.V1.PersonalAccountCreated>(
            account => account
                .UpdateOne
                .IdFromStream(stream => stream.GetId())
                .Update((evt, update) =>
                    update
                        .Set(x => x.Email, evt.Email)
                        .Set(x => x.Password, evt.Password)
                        .Set(x => x.SecurityQuestion, evt.SecurityQuestion)
                        .Set(x => x.SecurityAnswer, evt.SecurityAnswer)
                        .Set(x => x.HealthDataNotice, evt.HealthDataNotice)
                        .Set(x => x.TermsOfUser, evt.TermsOfUse)
                        .Set(x => x.State, evt.State)));
    }
}
