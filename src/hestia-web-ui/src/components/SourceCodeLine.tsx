import React, { CSSProperties } from "react";
import { Grid } from "@material-ui/core";

export interface SourceCodeLineProps {
  text: string;
  lineNumber: number;
}

export const SourceCodeLine = (props: SourceCodeLineProps) => {
  const rootStyle: CSSProperties = {
    textAlign: "left",
    fontSize: "1.3em"
  };

  const lineNumberStyle: CSSProperties = {
    fontFamily: "monospace",
    width: "10px",
    backgroundColor: "black"
  };

  const textStyle: CSSProperties = {
    fontFamily: "monospace"
  };

  if (!props.text || props.text === "") {
    return <div />;
  }

  return (
    <Grid container style={rootStyle} className="sourceLine">
      <Grid
        item
        xs={1}
        sm={1}
        style={{ backgroundColor: "black", paddingLeft: "15px" }}
      >
        <div style={lineNumberStyle}>{props.lineNumber}</div>
      </Grid>
      <Grid item xs={11} sm={11} style={{ paddingLeft: "10px" }}>
        <div style={textStyle}>{props.text}</div>
      </Grid>
    </Grid>
  );
};
