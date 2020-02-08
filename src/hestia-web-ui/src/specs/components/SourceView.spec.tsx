import React from "react";
import { render } from "@testing-library/react";
import { SourceView } from "../../components/SourceView";
import { dummyRepo } from "../../dummies/dummyData";

test("renders Source View", () => {
  const { getByText } = render(<SourceView Repository={dummyRepo} />);
  const linkElement = getByText(/Dummy/i);
  expect(linkElement).toBeInTheDocument();
});

test("renders Source Code component", () => {
  const { getByText } = render(<SourceView Repository={dummyRepo} />);
  const linkElement = getByText(/\//i);
  expect(linkElement).toBeInTheDocument();
});

test("renders Source Code component", () => {
  const { getByText } = render(<SourceView Repository={dummyRepo} />);
  const linkElement = getByText(/Please select a source file/i);
  expect(linkElement).toBeInTheDocument();
});
