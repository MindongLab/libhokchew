import { YngPingTwoSyllable, parseHandwriting } from "@hokchewjs/lib/models/yngping/yngping-two";

console.log(parseHandwriting("wa", null) );

console.log((parseHandwriting("wa", null) as YngPingTwoSyllable).getTypingForm());

//èi/ güong huà lǘ uāng wa`