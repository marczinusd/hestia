import React from "react";

export interface SourceCodeLineProps {
  text: string;
  lineNumber: number;
}

export const SourceCodeLine = (props: SourceCodeLineProps) => {
  return (
    <div>
      <p style={{ fontFamily: "monospaced" }}>{props.lineNumber}</p>
      <p style={{ fontFamily: "monospaced" }}>{props.text}</p>
    </div>
  );
};
