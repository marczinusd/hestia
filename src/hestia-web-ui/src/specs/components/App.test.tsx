import React from "react";
import { render } from "@testing-library/react";
import App from "../../components/App";

test("renders Source View", () => {
  const { getByText } = render(<App />);
  const linkElement = getByText(/Source View/i);
  expect(linkElement).toBeInTheDocument();
});

test("renders a header", () => {
  const { getByText } = render(<App />);
  const linkElement = getByText(/Hestia/i);
  expect(linkElement).toBeInTheDocument();
});
