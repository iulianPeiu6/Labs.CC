export const navigation = [
  {
    text: 'Home',
    path: '/home',
    icon: 'home'
  },
  {
    text: 'Workspaces',
    icon: 'folder',
    items: [
      {
        text: 'All',
        icon: 'folder',
        path: '/workspaces/all'
      },
      {
        text: 'Recent',
        icon: 'folder',
        path: '/workspaces/recent'
      },
      {
        text: 'Private',
        icon: 'folder',
        path: '/workspaces/private'
      },
      {
        text: 'Shared',
        icon: 'group',
        path: '/workspaces/shared'
      },
      {
        text: 'Favorite',
        icon: 'favorites',
        path: '/workspaces/favorites'
      }
    ]
  },
  {
    text: 'Create Workspace',
    icon: 'newfolder',
    path: '/workspaces/new'
  }
];
