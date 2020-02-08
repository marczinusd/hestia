import React, { useEffect, useState } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { dummySourceStructure } from "../dummies/dummyData";
import { renderStructure } from "./renderers/DirectoryRenderer";
import { Directory, File } from "../model/Repository";

export interface SourceStructureProps {
  rootDirectory: Directory;
  selectionChanged: (file: File | undefined) => void;
}

export const SourceStructure = (props: SourceStructureProps) => {
  const [selected, setSelected] = useState<File>();
  const useStyles = makeStyles({
    root: {
      flexGrow: 1
    }
  });

  useEffect(() => {
    props.selectionChanged(selected);
  }, [selected, props]);

  const classes = useStyles();

  return (
    <div>
      {renderStructure(dummySourceStructure, classes.root, f => {
        setSelected(f);
      })}
    </div>
  );
};
