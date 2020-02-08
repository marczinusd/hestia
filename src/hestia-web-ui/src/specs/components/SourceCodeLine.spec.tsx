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

test("container aligns text left", () => {
  const { getByText } = render(<SourceCodeLine lineNumber={1} text="hello" />);
  const container = getByText(/hello/).parentElement;
  expect(container).toHaveStyle("text-align: left");
});

test("renders line number in monospaced font", () => {
  const { getByText } = render(<SourceCodeLine lineNumber={1} text="hello" />);
  const lineNumber = getByText(/1/);
  expect(lineNumber).toHaveStyle("font-family: monospace");
});

test("renders text of line in monospaced font", () => {
  const { getByText } = render(<SourceCodeLine lineNumber={1} text="hello" />);
  const text = getByText(/hello/);
  expect(text).toHaveStyle("font-family: monospace");
});
