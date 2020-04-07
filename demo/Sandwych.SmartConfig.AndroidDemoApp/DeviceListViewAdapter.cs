using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Sandwych.SmartConfig.AndroidDemoApp
{
    public class DeviceListViewAdapter : BaseAdapter<ISmartConfigDevice>
    {
        private readonly IList<ISmartConfigDevice> _devices;

        public DeviceListViewAdapter(IList<ISmartConfigDevice> devices)
        {
            _devices = devices;
        }

        public override ISmartConfigDevice this[int position] => _devices[position];

        public override int Count => throw new NotImplementedException();

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.device_list_row, parent, false);

                /*
                var image = view.FindViewById<ImageView>(Resource.Id.photoImageView);
                var name = view.FindViewById<TextView>(Resource.Id.ipad);
                var department = view.FindViewById<TextView>(Resource.Id.departmentTextView);

                view.Tag = new DeviceListViewHolder()
                {
                    Image = image,
                    Name = name,
                    Department = department
                };
                var holder = (DeviceListViewHolder)view.Tag;
                holder.Photo.SetImageDrawable(ImageManager.Get(parent.Context, users[position].ImageUrl));
                holder.Name.Text = users[position].Name;
                holder.Department.Text = users[position].Department;
                return view;
                */
            }

            return view;
        }
    }

}