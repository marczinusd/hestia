import React from "react";
import { SourceCodeLine } from "./SourceCodeLine";
import { Typography } from "@material-ui/core";
import _ from "lodash";

export interface SourceCodeProps {
  source: string;
}

export const SourceCode = (props: SourceCodeProps) => {
  if (!props.source) {
    return <Typography>Please select a source file</Typography>;
  }

  const lines = props.source.split(/\r?\n/g);
  return (
    <div>
      {lines.map((line, index) => {
        return (
          <SourceCodeLine lineNumber={index + 1} text={line} key={index} />
        );
      })}
    </div>
  );
};
