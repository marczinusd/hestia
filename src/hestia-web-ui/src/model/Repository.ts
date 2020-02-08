export class Repository {
  private readonly _Name: string;
  private readonly _RootDir: Directory;

  constructor(name: string, directory: Directory) {
    this._Name = name;
    this._RootDir = directory;
  }

  get Name(): string {
    return this._Name;
  }

  get RootDirectory(): Directory {
    return this._RootDir;
  }
}

export type FileExtensions = ".js" | ".ts" | ".tsx" | "";

export class Directory {
  public Contents: Array<Directory | File>;

  get Path(): string {
    return this._Path;
  }

  constructor(path: string, contents: Array<Directory | File>) {
    this.Contents = contents;
    this._Path = path;
  }

  private readonly _Path: string;
}

export class File {
  constructor(
    content: string,
    path: string,
    extension: string | FileExtensions
  ) {
    this._Content = content;
    this._Extension = extension;
    this._Path = path;
  }

  get Content(): string {
    return this._Content;
  }

  get Path(): string {
    return this._Path;
  }

  get Extension(): string {
    return this._Extension;
  }

  private readonly _Content: string;
  private readonly _Path: string;
  private readonly _Extension: string;
}
