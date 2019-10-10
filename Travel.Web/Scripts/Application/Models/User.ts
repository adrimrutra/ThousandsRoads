/// <reference path="../_references.ts" />
module Application {
    export class User {
        Id: number;
        DisplayName: string;
        Email: string;
        Tokens: Array<Token>;
        FriendListItems: Array<FriendListItem>;
        Comments: Array<Comment>;
        Avatar: string;
    }
}  