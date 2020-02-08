import React from "react";
import { SourceCodeLine } from "./SourceCodeLine";

export interface SourceCodeProps {
  source: string;
}

export const SourceCode = (props: SourceCodeProps) => {
  return (
    <div>
      Source Code
      {props.source.split("\n").map((line, index) => {
        return (
          <SourceCodeLine lineNumber={index + 1} text={line} key={index} />
        );
      })}
    </div>
  );
};
