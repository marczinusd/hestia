import React, { useState } from "react";
import { SourceStructure } from "./SourceStructure";
import { SourceCode } from "./SourceCode";
import { createStyles, Theme } from "@material-ui/core";
import makeStyles from "@material-ui/core/styles/makeStyles";
import Grid from "@material-ui/core/Grid";
import Paper from "@material-ui/core/Paper";
import { Repository, File } from "../model/Repository";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1
    },
    paper: {
      padding: theme.spacing(2),
      textAlign: "center",
      color: theme.palette.text.secondary
    }
  })
);

export interface SourceViewProps {
  Repository: Repository;
}

export const SourceView = (props: SourceViewProps) => {
  const [selected, setSelected] = useState<File>();
  const classes = useStyles();

  return (
    <div>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Paper className={classes.paper}>{props.Repository.Name}</Paper>
        </Grid>
        <Grid item xs={6} sm={3}>
          <Paper className={classes.paper}>
            <SourceStructure
              rootDirectory={props.Repository.RootDirectory}
              selectionChanged={f => setSelected(f)}
            />
          </Paper>
        </Grid>
        <Grid item xs={6} sm={9}>
          <Paper className={classes.paper}>
            <SourceCode source={selected?.Content ?? ""} />
          </Paper>
        </Grid>
      </Grid>
    </div>
  );
};
