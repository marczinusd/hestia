import React from "react";
import { render } from "@testing-library/react";
import { SourceCode } from "../../components/SourceCode";
import "@testing-library/jest-dom/extend-expect";
import { dummyFile } from "../../dummies/dummyData";

test("renders 3 lines of code for dummy file", () => {
  const { getAllByText } = render(<SourceCode file={dummyFile} />);
  const codeLines = getAllByText(/bla/i);
  expect(codeLines.length).toBe(3);
});
