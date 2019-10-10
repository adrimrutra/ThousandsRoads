/// <reference path="../_references.ts" />
module Application {
    export class UserService extends Service<User>
    {
        private $superProvider: IResource;

        constructor($resource: IResourceService) {
            super('User', $resource);
            this.$superProvider = $resource('Api/User/:method/:id', { method: '@method', id: '@Id' },
                {
                    update: { method: 'PUT' }
                });
        }
        GetSocialid(token: Token, success: (models: User) => void, error: (reasone) => void) {
            this.$superProvider.get({ id: token.SocialId, method: "socialid", type : token.Tokentype}, success, error);
        }

        PostComment(comment: Comment, success: (model: Comment) => void, error: (reasone) => void)
        {
            this.$superProvider.save({ method: "comment" }, comment, success, error);
        }

        NotifyPassenger(driver: number, travel: number, success:(flag: boolean) => void, error: (reasone) => void) {
            this.$superProvider.get({ travel: travel, driver: driver, method: "notifypassenger" },success, error);
        } 
        NotifyDriver(driver: number, travel: number, success: (flag: boolean) => void, error: (reasone) => void) {
            this.$superProvider.get({ travel: travel, driver: driver, method: "notifydriver" }, success, error);
        }
        GetFriends(driver: number,travel: number,  success: (model :TravelFriends) => void, error: (reasone) => void) {
            this.$superProvider.get({ travel: travel, driver:driver, method: "friends" }, success, error);
        }

        GetMessages(success: (models: Array<Message>) => void, error: (reasone) => void) {
            this.$superProvider.query({ method: "messages" }, success, error);
        }
        DeleteMessages(deletedIds: Array<number>,success: () => void, error: (reasone) => void) {
            this.$superProvider.delete({ deletedIds: deletedIds , method: "messages" }, success, error);
        }
        PutMessage(message: Message, success: (message: Message) => void, error: (reasone) => void) {
            this.$superProvider.update({ id: message.Id, method: "message" }, message, success, error);
        }
        PostMessage(message: Message, success: (message: Message) => void, error: (reasone) => void) {
            this.$superProvider.save({method: "messagepost" },message, success, error);
        }
        PostCallBack(message: Message, success: (flag: boolean) => void, error: (reasone) => void) {
            this.$superProvider.save({ method: "callback" }, message, success, error);
        }

        GetCurenUser(success: (model: User) => void, error: (reasone) => void) {
            this.$superProvider.get({ method: "user" }, success, error);
        }

        
    }
} 