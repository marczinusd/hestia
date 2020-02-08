import React from "react";
import { Breadcrumbs, createStyles, Theme } from "@material-ui/core";
import { File } from "../model/Repository";
import Link from "@material-ui/core/Link";
import { InsertDriveFile } from "@material-ui/icons";
import FolderIcon from "@material-ui/icons/Folder";
import { makeStyles } from "@material-ui/core/styles";

export interface SourceCodeBreadCrumbProps {
  file: File;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    link: {
      display: "flex"
    },
    icon: {
      marginRight: theme.spacing(0.5),
      width: 20,
      height: 20
    },
    crumb: {
      marginBottom: theme.spacing(2)
    }
  })
);

export const SourceCodeBreadCrumb = (props: SourceCodeBreadCrumbProps) => {
  const classes = useStyles();

  return (
    <Breadcrumbs className={classes.crumb}>
      {props.file.Path.split("/").map((crumb, index) => {
        if (!crumb) {
          return "";
        }

        return (
          <Link color="inherit" href="#" className={classes.link} key={index}>
            <FolderIcon className={classes.icon} />
            {crumb}
          </Link>
        );
      })}
      <Link color="inherit" href="#" className={classes.link}>
        <InsertDriveFile className={classes.icon} />
        {props.file.Filename + props.file.Extension}
      </Link>
    </Breadcrumbs>
  );
};
