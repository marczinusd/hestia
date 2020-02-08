import React from "react";
import { render } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import { SourceCodeBreadCrumb } from "../../components/SourceCodeBreadcrumb";
import { dummyFile } from "../../dummies/dummyData";

test("renders crumb containing documents", () => {
  const { getByText } = render(<SourceCodeBreadCrumb file={dummyFile} />);
  const linkElement = getByText(/documents/i);
  expect(linkElement).toBeInTheDocument();
});
