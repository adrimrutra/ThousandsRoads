/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var Comment = (function () {
        function Comment() {
        }
        return Comment;
    })();
    Application.Comment = Comment;
    (function (CommentType) {
        CommentType[CommentType["Positive"] = 1] = "Positive";
        CommentType[CommentType["Neutral"] = 2] = "Neutral";
        CommentType[CommentType["Negative"] = 3] = "Negative";
    })(Application.CommentType || (Application.CommentType = {}));
    var CommentType = Application.CommentType;
})(Application || (Application = {}));
//# sourceMappingURL=Comment.js.map