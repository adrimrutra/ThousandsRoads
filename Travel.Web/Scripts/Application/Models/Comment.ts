/// <reference path="../_references.ts" />
module Application {
    export class Comment {
        UserId: number;
        User: User;
        MessengerId: number;
        Messenger: User;
        Message: string;
        Type: CommentType;
        Data: Date;
    }
    export enum CommentType {
        Positive = 1, Neutral = 2, Negative=3
    }
} 