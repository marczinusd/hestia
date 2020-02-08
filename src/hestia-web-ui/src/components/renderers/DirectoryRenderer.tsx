import { Directory, File } from "../../model/Repository";
import TreeItem from "@material-ui/lab/TreeItem";
import TreeView from "@material-ui/lab/TreeView";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import React from "react";

export const renderStructure = (
  dir: Directory,
  className: string = "",
  onSelected: (f: File) => void
) => {
  let currentNodeId = 1;
  const renderDirectory = (dir: Directory) => {
    return (
      <TreeItem
        nodeId={(currentNodeId++).toString()}
        label={dir.Path}
        key={currentNodeId}
      >
        {dir.Contents.map(c => {
          if (c instanceof File) {
            return renderFile(c);
          } else {
            return renderDirectory(c);
          }
        })}
      </TreeItem>
    );
  };

  const renderFile = (file: File) => {
    return (
      <TreeItem
        nodeId={(currentNodeId++).toString()}
        label={file.Filename + file.Extension}
        onFocus={_ => onSelected(file)}
        key={currentNodeId}
      />
    );
  };

  return (
    <TreeView
      className={className}
      defaultCollapseIcon={<ExpandMoreIcon />}
      defaultExpandIcon={<ChevronRightIcon />}
    >
      {renderDirectory(dir)}
    </TreeView>
  );
};
