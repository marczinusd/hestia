import React from "react";
import { render } from "@testing-library/react";
import { SourceView } from "../../components/SourceView";

test("renders Source View", () => {
  const { getByText } = render(<SourceView />);
  const linkElement = getByText(/Source View/i);
  expect(linkElement).toBeInTheDocument();
});

test("renders Source Code component", () => {
  const { getByText } = render(<SourceView />);
  const linkElement = getByText(/Source Structure/i);
  expect(linkElement).toBeInTheDocument();
});

test("renders Source Code component", () => {
  const { getByText } = render(<SourceView />);
  const linkElement = getByText(/Source Code/i);
  expect(linkElement).toBeInTheDocument();
});
