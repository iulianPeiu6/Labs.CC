import authConfig from "../../auth-config.json"

export const environment = {
  production: false,
  auth: {
    domain: authConfig.domain,
    clientId: authConfig.clientId,
    redirectUri: window.location.origin
  },
  apiEndpoint: "https://localhost:5001/"
};
