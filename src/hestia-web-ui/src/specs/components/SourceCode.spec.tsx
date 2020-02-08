import React from "react";
import { render } from "@testing-library/react";
import { SourceCode } from "../../components/SourceCode";
import "@testing-library/jest-dom/extend-expect";

test("renders a line of code", () => {
  const { getByText } = render(<SourceCode source="bla" />);
  const linkElement = getByText(/bla/i);
  expect(linkElement).toBeInTheDocument();
});
