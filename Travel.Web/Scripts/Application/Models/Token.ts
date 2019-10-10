/// <reference path="../_references.ts" />
module Application {

    export class Token {
        Id: Number;
        Tokentype: TokenType;
        SocialId: string;
        UserId: Number;
        User: User;
        Email: string;
        Access: string;
    }


    export enum TokenType {
        facebook = 1, vkontakte = 2, odnoklassniku = 3
    }
}  