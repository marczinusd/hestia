import React from "react";
import { render } from "@testing-library/react";
import { SourceStructure } from "../../components/SourceStructure";

test("renders Source Code", () => {
  const { getByText } = render(<SourceStructure />);
  const linkElement = getByText(/Source Structure/i);
  expect(linkElement).toBeInTheDocument();
});
