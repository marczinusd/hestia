/// <reference types="Cypress" />

it("visits the app", () => {
    cy.visit("/");
});

it("has element that contains 'Learn React'", () => {
    cy.visit("").contains<HTMLAnchorElement>("Learn React");
});
