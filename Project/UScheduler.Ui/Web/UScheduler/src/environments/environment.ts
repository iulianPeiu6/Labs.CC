import authConfig from "../../auth-config.json"

export const environment = {
  production: false,
  apiEndpoint: "https://localhost:5001",
  auth: {
    domain: authConfig.domain,
    clientId: authConfig.clientId,
    redirectUri: window.location.origin,
    audience: authConfig.audience
  }
};
