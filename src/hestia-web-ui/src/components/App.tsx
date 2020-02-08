import React from "react";
import CssBaseline from "@material-ui/core/CssBaseline";
import { createStyles, Theme, Typography } from "@material-ui/core";
import { makeStyles } from "@material-ui/core/styles";
import MenuIcon from "@material-ui/icons/Menu";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Container from "@material-ui/core/Container";
import IconButton from "@material-ui/core/IconButton";
import Button from "@material-ui/core/Button";
import createMuiTheme from "@material-ui/core/styles/createMuiTheme";
import useMediaQuery from "@material-ui/core/useMediaQuery";
import { ThemeProvider } from "@material-ui/core/styles";
import { SourceView } from "./SourceView";
import { dummyRepo } from "../dummies/dummyData";

function App() {
  const prefersDarkMode = useMediaQuery("(prefers-color-scheme: dark)");
  const darkModeOverride = true;

  const theme = React.useMemo(
    () =>
      createMuiTheme({
        palette: {
          type: darkModeOverride || prefersDarkMode ? "dark" : "light"
        }
      }),
    [prefersDarkMode, darkModeOverride]
  );

  const useStyles = makeStyles((theme: Theme) =>
    createStyles({
      root: {
        flexGrow: 1
      },
      menuButton: {
        marginRight: theme.spacing(2)
      },
      title: {
        flexGrow: 1
      }
    })
  );

  const classes = useStyles();

  return (
    <ThemeProvider theme={theme}>
      <Container className="App">
        <CssBaseline />
        <AppBar position="static">
          <Toolbar>
            <IconButton
              edge="start"
              className={classes.menuButton}
              color="inherit"
              aria-label="menu"
            >
              <MenuIcon />
            </IconButton>
            <Typography variant="h6" className={classes.title}>
              Hestia
            </Typography>
            <Button color="inherit">Login</Button>
          </Toolbar>
        </AppBar>
        <SourceView Repository={dummyRepo} />
      </Container>
    </ThemeProvider>
  );
}

export default App;
