import React from "react";
import { render } from "@testing-library/react";
import { SourceCode } from "../../components/SourceCode";

test("renders Source Code", () => {
  const { getByText } = render(<SourceCode source="blablabla" />);
  const linkElement = getByText(/Source Code/i);
  expect(linkElement).toBeInTheDocument();
});
