import { Directory, File, Repository } from "../model/Repository";

export const dummySourceStructure = new Directory("/", [
  new Directory("Applications", [
    new File("bla", "/applications/", "Calendar", ""),
    new File("bla", "/applications/", "Chrome", ""),
    new File("bla", "/applications/", "WebStorm", "")
  ]),
  new Directory("Documents", [
    new Directory("Material-UI", [
      new Directory("src", [
        new File(
          "bla\nbla\nbla",
          "/Documents/material-ui/src/",
          "index",
          ".js"
        ),
        new File(
          "ble\nble\nble\nble",
          "/Documents/material-ui/src/",
          "tree-view",
          ".js"
        )
      ])
    ])
  ])
]);

export const dummyFile = new File(
  "bla\nbla\nbla",
  "/Documents/material-ui/src/",
  "index",
  ".js"
);
export const dummyRepo = new Repository("Dummy", dummySourceStructure);
