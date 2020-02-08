import React from "react";
import { render } from "@testing-library/react";
import { SourceCodeLine } from "../../components/SourceCodeLine";

test("renders text of line", () => {
  const { getByText } = render(<SourceCodeLine lineNumber={1} text="hello" />);
  const text = getByText(/hello/i);
  expect(text).toBeInTheDocument();
});

test("renders line number", () => {
  const { getByText } = render(<SourceCodeLine lineNumber={1} text="hello" />);
  const lineNumber = getByText(/1/i);
  expect(lineNumber).toBeInTheDocument();
});
