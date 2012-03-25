using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rackspace.Cloudfiles;

namespace Sitereactor.CloudProviders.CloudFiles
{
    public class Factory
    {
        private readonly Client _client;
        private readonly CF_Account _account;
        private readonly CF_Connection _connection;

        public Factory(string username, string apiKey)
        {
            var creds = new UserCredentials(username, apiKey);
            _client = new Client();
            _connection = new CF_Connection(creds, _client);
            _account = new CF_Account(_connection, _client);
        }

        /// <summary>
        /// Gets a list of all containers
        /// </summary>
        /// <returns>A list of all containers as <see cref="string"/></returns>
        public List<string> GetAllContainersAsSimpleList()
        {
            var containers = _account.GetContainers(true);
            return containers.Select(x => x.Name).ToList();
        }

        /// <summary>
        /// Get all containers with all information
        /// </summary>
        /// <returns>A list of all containers as <see cref="Container"/> objects</returns>
        public List<Container> GetContainers()
        {
            var containers = _account.GetContainers(true);
            return containers;
        }

        /// <summary>
        /// Gets a list of items in a container
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <returns>A list of items</returns>
        public List<StorageObject> GetAllItemsByContainer(string containerName)
        {
            var container = new CF_Container(_connection, _client, containerName);
            return container.GetObjects(true);
        }

        /// <summary>
        /// Get information for an item in a container
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <param name="itemName">Name of item in Container</param>
        /// <returns><see cref="CloudFile"/></returns>
        public StorageObject GetStorageItemInformation(string containerName, string itemName)
        {
            var container = new CF_Container(_connection, _client, containerName);
            var storageObject = container.GetObject(itemName);
            return storageObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public Container GetContainerInformation(string containerName)
        {
            var container = new CF_Container(_connection, _client, containerName);
            return container;
        }

        /// <summary>
        /// Gets the public url for the container
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <returns>Url for public container</returns>
        public string GetCdnUriForContainer(string containerName)
        {
            var container = new CF_Container(_connection, _client, containerName);
            return container.CdnUri.AbsoluteUri;
        }

        /// <summary>
        /// Gets the public urls for an item
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <param name="remoteStorageItemName">Name of the item in Storage</param>
        /// <returns>Url for item in CDN</returns>
        public string GetCdnUriForItem(string containerName, string remoteStorageItemName)
        {
            var container = new CF_Container(_connection, _client, containerName);
            var storageObject = container.GetObject(remoteStorageItemName);
            return storageObject.CdnUri.AbsoluteUri;
        }

        /// <summary>
        /// Create a container
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        public void CreateContainer(string containerName)
        {
            _account.CreateContainer(containerName);
        }

        /// <summary>
        /// Mark a container as public (available on the CDN)
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <returns><see cref="Uri"/> to the public container</returns>
        public Uri SetPublicContainer(string containerName)
        {
            var container = new CF_Container(_connection, _client, containerName);
            container.MakePublic();
            return container.CdnUri;
        }

        /// <summary>
        /// Mark a container as public (available on the CDN), with time-to-live (TTL) parameters
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <param name="timeToLiveInSeconds"></param>
        /// <returns><see cref="Uri"/> to the public container</returns>
        public Uri SetPublicContainer(string containerName, int timeToLiveInSeconds)
        {
            var container = new CF_Container(_connection, _client, containerName);
            container.MakePublic(timeToLiveInSeconds);
            return container.CdnUri;
        }

        /// <summary>
        /// Mark a container as private (remove it from the CDN)
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        public void SetPrivateContainer(string containerName)
        {
            var container = new CF_Container(_connection, _client, containerName);
            container.MakePrivate();
        }

        /// <summary>
        /// Puts a file as <see cref="Stream"/> into a container
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <param name="storageStream">File as <see cref="Stream"/> to upload</param>
        /// <param name="remoteStorageItemName">Name of the item in Storage</param>
        public void PutItemInContainer(string containerName, Stream storageStream, string remoteStorageItemName)
        {
            var container = new CF_Container(_connection, _client, containerName);
            var storageObject = container.CreateObject(remoteStorageItemName);
            storageObject.Write(storageStream);
        }

        /// <summary>
        /// Puts a file as <see cref="Stream"/> into a container with metadata
        /// </summary>
        /// <param name="metadata">Dictionary of strings with metadata</param>
        /// <param name="containerName">Name of Container</param>
        /// <param name="storageStream">File as <see cref="Stream"/> to upload</param>
        /// <param name="remoteStorageItemName">Name of the item in Storage</param>
        public void PutItemInContainer(Dictionary<string, string> metadata, string containerName, Stream storageStream,
                                       string remoteStorageItemName)
        {
            var container = new CF_Container(_connection, _client, containerName);
            var storageObject = container.CreateObject(remoteStorageItemName);
            storageObject.Write(storageStream, metadata);
        }

        /// <summary>
        /// Deletes an item from a container
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <param name="remoteStorageItemName">Name of the item in Storage</param>
        public void DeleteItemFromContainer(string containerName, string remoteStorageItemName)
        {
            var container = new CF_Container(_connection, _client, containerName);
            container.DeleteObject(remoteStorageItemName);
        }

        /// <summary>
        /// Delete a container
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        public void DeleteContainer(string containerName)
        {
            _account.DeleteContainer(containerName);
        }
    }
}