/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var Token = (function () {
        function Token() {
        }
        return Token;
    })();
    Application.Token = Token;
    (function (TokenType) {
        TokenType[TokenType["facebook"] = 1] = "facebook";
        TokenType[TokenType["vkontakte"] = 2] = "vkontakte";
        TokenType[TokenType["odnoklassniku"] = 3] = "odnoklassniku";
    })(Application.TokenType || (Application.TokenType = {}));
    var TokenType = Application.TokenType;
})(Application || (Application = {}));
//# sourceMappingURL=Token.js.map