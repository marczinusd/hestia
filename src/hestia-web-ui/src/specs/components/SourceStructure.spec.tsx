import React from "react";
import { render } from "@testing-library/react";
import { SourceStructure } from "../../components/SourceStructure";
import { dummyRepo } from "../../dummies/dummyData";

test("renders root directory for dummy structure", () => {
  const { getByText } = render(
    <SourceStructure
      rootDirectory={dummyRepo.RootDirectory}
      selectionChanged={f => null}
    />
  );
  const linkElement = getByText(/\//i);
  expect(linkElement).toBeInTheDocument();
});
