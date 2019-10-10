/// <reference path="../_references.ts" />
var Application;
(function (Application) {
    var Message = (function () {
        function Message() {
        }
        return Message;
    })();
    Application.Message = Message;
    (function (LuggageType) {
        LuggageType[LuggageType["Default"] = 0] = "Default";
        LuggageType[LuggageType["Min"] = 1] = "Min";
        LuggageType[LuggageType["Norm"] = 2] = "Norm";
        LuggageType[LuggageType["Max"] = 3] = "Max";
    })(Application.LuggageType || (Application.LuggageType = {}));
    var LuggageType = Application.LuggageType;
    (function (DirectionType) {
        DirectionType[DirectionType["One"] = 1] = "One";
        DirectionType[DirectionType["Two"] = 2] = "Two";
    })(Application.DirectionType || (Application.DirectionType = {}));
    var DirectionType = Application.DirectionType;
    (function (MessageType) {
        MessageType[MessageType["Subscribe"] = 1] = "Subscribe";
        MessageType[MessageType["Accept"] = 2] = "Accept";
        MessageType[MessageType["Decline"] = 3] = "Decline";
        MessageType[MessageType["Inner"] = 4] = "Inner";
        MessageType[MessageType["Outer"] = 5] = "Outer";
    })(Application.MessageType || (Application.MessageType = {}));
    var MessageType = Application.MessageType;
})(Application || (Application = {}));
//# sourceMappingURL=Message.js.map