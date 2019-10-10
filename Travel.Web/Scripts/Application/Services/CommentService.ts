/// <reference path="../_references.ts" />
module Application {
    export class CommentService extends Service<Comment>
    {
        constructor($resource: IResourceService) {
            super('Comment', $resource);
        }
    }
}  