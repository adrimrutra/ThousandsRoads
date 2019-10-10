var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var CommentService = (function (_super) {
        __extends(CommentService, _super);
        function CommentService($resource) {
            _super.call(this, 'Comment', $resource);
        }
        return CommentService;
    })(Application.Service);
    Application.CommentService = CommentService;
})(Application || (Application = {}));
//# sourceMappingURL=CommentService.js.map