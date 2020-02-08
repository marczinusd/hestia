import React from "react";
import { SourceStructure } from "./SourceStructure";
import { SourceCode } from "./SourceCode";
import { createStyles, Theme } from "@material-ui/core";
import makeStyles from "@material-ui/core/styles/makeStyles";
import Grid from "@material-ui/core/Grid";
import Paper from "@material-ui/core/Paper";

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

export const SourceView = () => {
  const classes = useStyles();

  return (
    <div>
      Source View
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Paper className={classes.paper}>Current Repository</Paper>
        </Grid>
        <Grid item xs={6} sm={3}>
          <Paper className={classes.paper}>
            <SourceStructure />
          </Paper>
        </Grid>
        <Grid item xs={6} sm={9}>
          <Paper className={classes.paper}>
            <SourceCode source="blablabla" />
          </Paper>
        </Grid>
      </Grid>
    </div>
  );
};
