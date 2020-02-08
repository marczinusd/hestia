import { Directory, File, Repository } from "../model/Repository";

export const dummySourceStructure = new Directory("/", [
  new Directory("Applications", [
    new File("bla", "Calendar", ""),
    new File("bla", "Chrome", ""),
    new File("bla", "WebStorm", "")
  ]),
  new Directory("Documents", [
    new Directory("Material-UI", [
      new Directory("src", [
        new File("bla\nbla\nbla", "index", ".js"),
        new File("ble\nble\nble\nble", "tree-view", ".js")
      ])
    ])
  ])
]);

export const dummyRepo = new Repository("Dummy", dummySourceStructure);
