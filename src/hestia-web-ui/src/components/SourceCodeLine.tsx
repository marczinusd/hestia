import React, { CSSProperties } from "react";

export interface SourceCodeLineProps {
  text: string;
  lineNumber: number;
}

export const SourceCodeLine = (props: SourceCodeLineProps) => {
  const rootStyle: CSSProperties = {
    textAlign: "left",
    fontSize: "1em"
  };

  const lineNumberStyle: CSSProperties = {
    fontFamily: "monospace",
    width: "10px"
  };

  const textStyle: CSSProperties = {
    fontFamily: "monospace"
  };

  return (
    <div style={rootStyle} className="sourceLine">
      <span style={lineNumberStyle}>{props.lineNumber}</span>
      <span style={textStyle}>{props.text}</span>
    </div>
  );
};
