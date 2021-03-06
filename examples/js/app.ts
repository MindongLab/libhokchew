import { YngPingTwoSyllable, ParseError, parseHandwriting } from "@hokchewjs/lib/models/yngping/yngping-two";

let txtInput: HTMLTextAreaElement = document.querySelector("#txtInput");
let txtOutput: HTMLTextAreaElement = document.querySelector("#txtOutput");
let toggleUpperScript: HTMLInputElement = document.querySelector("#toggleUpperScript");
var shouldReplace = false;

txtInput.oninput = () => {
    let txt = txtInput.value;
    txtOutput.value = convert(txt);
}

shouldReplace = toggleUpperScript.checked;
toggleUpperScript.onchange = () => {
    shouldReplace = toggleUpperScript.checked;
    let txt = txtInput.value;
    txtOutput.value = convert(txt);
}

function convert(s: string): string {
    return tokenize(s).map(t => {
        let tryParse = parseHandwriting(t);
        if ((tryParse as ParseError).message) {
            return t;
        } else {
            return maybeReplaceWithUpperScript(
                (tryParse as YngPingTwoSyllable).getTypingForm());
        }
    }).join('');
}


function maybeReplaceWithUpperScript(s: string) {
    function replaceAll(str, find, replace) {
        return str.replace(new RegExp(find, 'g'), replace);
    }
    if (!shouldReplace) return s;
    s = replaceAll(s, '1','¹');
    s = replaceAll(s, '2','²');
    s = replaceAll(s, '3','³');
    s = replaceAll(s, '4','⁴');
    s = replaceAll(s, '5','⁵');
    s = replaceAll(s, '7','⁷');
    s = replaceAll(s, '8','⁸');
    return s;
}

function tokenize(s: string): string[] {
    let output = [];
    let currentToken = '';
    let currentType: TokenType = null;

    function commit() {
        if (currentType == null) return;
        output.push(currentToken);
        currentToken = '';
        currentType = null;
    }
    for (let i = 0; i < s.length; ++i) {
        let newType = getTokenType(s[i]);
        if (newType != currentType) {
            commit();
            currentType = newType;
            currentToken += s[i];
            if (currentType == TokenType.SINGLE_SYMBOL) {
                // commit immediately
                commit();
            }
        } else {
            currentToken += s[i];
        }
    }
    commit();
    return output;
}

const RE_CJK = /[⺀-\u2efeㆠ-\u31be㇀-\u31ee㈀-㋾㌀-㏾㐀-\u4dbe一-\u9ffe豈-\ufafe︰-﹎]|[\ud840-\ud868\ud86a-\ud86c][\udc00-\udfff]|\ud869[\udc00-\udede\udf00-\udfff]|\ud86d[\udc00-\udf3e\udf40-\udfff]|\ud86e[\udc00-\udc1e]|\ud87e[\udc00-\ude1e]/

function getTokenType(c: string) {
    if (c == ' ' || c == '\t' || c == '\r' || c=='\n') {
        return TokenType.WHITESPACES;
    }
    else if (SINGLE_SYMBOLS.has(c)) {
        return TokenType.SINGLE_SYMBOL;
    }
    else if (RE_CJK.test(c)) {
        return TokenType.CJK
    }
    return TokenType.OTHER;
}



const SINGLE_SYMBOLS = new Set([
    '`',
    '"',
    "'",
    "\\",
    "/",
    ".",
    ",",
    '-',
    '_',
    '—',
    '，',
    '。'
])
enum TokenType {
    WHITESPACES,
    SINGLE_SYMBOL, // `",._-/\
    CJK,
    OTHER,
}