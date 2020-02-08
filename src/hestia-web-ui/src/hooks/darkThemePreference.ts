import React from "react";
import createMuiTheme from "@material-ui/core/styles/createMuiTheme";
import useMediaQuery from "@material-ui/core/useMediaQuery";

const prefersDarkMode = useMediaQuery("(prefers-color-scheme: dark)");

export const prefersDarkTheme = () =>
  React.useMemo(
    () =>
      createMuiTheme({
        palette: {
          type: prefersDarkMode ? "dark" : "light"
        }
      }),
    [prefersDarkMode]
  );
