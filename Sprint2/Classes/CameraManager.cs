using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Author: Tommy Kasarcik
 */

namespace Sprint5
{
	/*
	 * Singleton class to handle creation and storage of all <c> Camera </c> objects
	 */
	public class CameraManager
	{

		/*
		 * Private dictionary of all <c> Cameras </c>
		 */
		private Dictionary<IGameObject, Camera> cameraDict;

		/*
		 * Private Instance of <c> this </c>. 
		 */
		private static CameraManager instance = new CameraManager();

		/*
         * Public Instance of <c> this </c>.
         */
		public static CameraManager Instance
		{
			get
			{
				return instance;
			}
		}

		/*
         * Private Constructor so <c> this </c> cannot be instantiated outside of <c> this </c>.
         */
		private CameraManager()
		{
			cameraDict = new Dictionary<IGameObject, Camera>();
		}

		/*
		 * Public Methods
		 */

		/*
		 * Create a new <c> Camera </c> and add it to <c> cameraList </c>
		 */
		public void CreateCamera(IGameObject target)
		{
			cameraDict.Add(target, new Camera(target));
		}

		/*
		 * Delete the camera of the specified target.
		 */
		public void DeleteCamera(IGameObject target)
		{
			if (cameraDict.ContainsKey(target))
			{
				cameraDict.Remove(target);
			}
		}

		/*
		 * Return a List containing all <c> Camera </c> objects
		 */
		public List<Camera> GetList()
		{
			return cameraDict.Values.ToList();
		}

		/*
		 * Return the <c> Camera </c> that follows the specified <c> IGameObject </c> if it exists,
		 * and null otherwise.
		 */
		public Camera GetCamera(IGameObject target)
		{
			Camera ret = null;

			if (cameraDict.ContainsKey(target))
			{
				ret = cameraDict[target];
			}

			return ret;
		} 
	}
}
