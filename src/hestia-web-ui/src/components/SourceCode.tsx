import React from "react";
import { SourceCodeLine } from "./SourceCodeLine";
import { Typography } from "@material-ui/core";
import { SourceCodeBreadCrumb } from "./SourceCodeBreadcrumb";
import { File } from "../model/Repository";

export interface SourceCodeProps {
  file: File | undefined;
}

export const SourceCode = (props: SourceCodeProps) => {
  if (!props.file || !props.file.Content) {
    return <Typography>Please select a source file</Typography>;
  }

  const lines = props.file.Content.split(/\r?\n/g);
  return (
    <div>
      <SourceCodeBreadCrumb file={props.file} />
      <div style={{ backgroundColor: "#424242" }}>
        {lines.map((line, index) => {
          return (
            <SourceCodeLine lineNumber={index + 1} text={line} key={index} />
          );
        })}
      </div>
    </div>
  );
};
